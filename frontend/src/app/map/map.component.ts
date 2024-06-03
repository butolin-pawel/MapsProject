import { Component, ElementRef, ViewChild } from '@angular/core';
import { MapService } from '../map.service';
import * as L from 'leaflet';
import { RouteService } from '../service/route.service';
import 'leaflet-routing-machine';
import { BrowserModule } from '@angular/platform-browser';
import { delay } from 'rxjs';
import { faLocationArrow } from '@fortawesome/free-solid-svg-icons/faLocationArrow';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlaceService } from '../service/place.service';
import { Route } from '../class/route';
import { Place } from '../class/place';
import { ActivatedRoute, Router } from '@angular/router';
import { StarRatingComponent } from '../star-rating/star-rating.component';
import { FeedbackService } from '../service/feedback.service';
import { Feedback } from '../class/feedback';
@Component({
  selector: 'app-map',
  standalone: true,
  imports: [ ToastrModule,CommonModule, FormsModule,StarRatingComponent],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent {
  @ViewChild('map') mapElement!: ElementRef;

  constructor(private mapService: MapService, private routeService : RouteService, private toast : ToastrService,private placeService : PlaceService,private route : ActivatedRoute,private router: Router,private feedService : FeedbackService) {
    this.rut  =  route.snapshot.queryParams['rut'];

   }
  rut : number;
  fillFeedBack : boolean = false;
  private map!: L.Map;
  private userMarker!: L.Marker;
  marker: any;
  score : number = 1;
  feedBackText : string = "";
  watchId! : number;
  way : any;
  listPlaces : Place[] = [];
  creationRoutesPlace : number[] = [];
  finishCreate : boolean = false;
  feedBack : boolean = false;
  nameNewRoute : string = '';
  descNewRoute : string = '';
  action : string = 'choose';
  placesInRoute : any[] = [];
  private userIcon = L.icon({
    iconUrl: '../../assets/circle-regular.svg', // путь к вашему изображению иконки
    iconSize: [50, 50], // размер иконки
    iconAnchor: [25, 25], // точка якоря иконки (центр нижнего края иконки)
    popupAnchor: [0, -50], // точка всплывающего окна относительно иконки
  });
  ngOnInit(): void {
    this.watchId = navigator.geolocation.watchPosition((position) => {
      const lat = position.coords.latitude;
      const lng = position.coords.longitude;
      this.updateUserMarker(lat, lng);
    }, (error) => {
    }, {
      enableHighAccuracy: true,
      maximumAge: 0,
      timeout: 5000
    });
  }

  ngAfterViewInit(){
    this.initializeMap();
    if(this.rut){
        this.routeStart(this.rut);
        this.action = 'walking';
    }
    else{
      this.showPlaces();
    }
  }
  ngOnDestroy(): void {
    // Остановка отслеживания позиции при уничтожении компонента
    if (this.watchId) {
      navigator.geolocation.clearWatch(this.watchId);
    }
  }
  showPlaces(){
    this.placeService.getAllPlace().subscribe((res)=>{
      this.listPlaces = res;
      this.listPlaces.forEach(elem =>{
        this.addMarkers(elem);
      })
    });

  }
  changeAction(act : string){
    this.action = act;
    if(act === 'creating' || act === 'addPlace' || act === 'choose')
      {
        this.map.on('click', (e) => {
          this.onMapClick(e);
        });
      }
  }
  getPosition(): Promise<GeolocationPosition> {
    return new Promise((resolve, reject) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(resolve, reject);
      } else {
        reject(new Error('Geolocation is not supported by this browser.'));
      }
    });
  }

  initializeMap(){
    let center: L.LatLngLiteral;
    this.getPosition()
    .then( pos => {
      center = { lat: pos.coords.latitude, lng: pos.coords.longitude};
      const zoom = 16;

      this.map = L.map(this.mapElement.nativeElement).setView(center, zoom);

      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',{
      })
      .addTo(this.map);
      this.addUserMarker(pos.coords.latitude,pos.coords.longitude);
    })
    .catch(err => {
      console.error(err);
    });

  }

  addMarkers(elem : Place): void {
    const pointBlock = document.createElement('div');
    const descriptionPoint =  document.createElement('p');
    descriptionPoint.append(elem.name+'\n'+elem.description+'\n'+elem.adress);
    descriptionPoint.className = "text-lg text-wrap"
    const addToRoute = document.createElement('button');
    addToRoute.innerHTML = "Добавить в маршрут";
    addToRoute.onclick = () =>{
      if(this.action === 'creating')
        this.addToRoute(elem);
    };
    pointBlock.appendChild(descriptionPoint);
    pointBlock.appendChild(addToRoute);
    let point =  L.marker([elem.latitude, elem.longitude]).bindPopup(pointBlock);
    this.placesInRoute.push(point);
    point.addTo(this.map);
  }

  onMapClick(e: any) {

    if (this.marker) {
      this.map.removeLayer(this.marker);
      this.marker = null;
      return;
    }

    // Обработка клика на карту
    this.marker = L.marker(e.latlng).addTo(this.map);
    const buttonContainer = document.createElement('div');
    buttonContainer.className = "flex flex-col text-lg "

    // Создание кнопок
    const addButton = document.createElement('button');
    addButton.innerHTML = 'Ввести данные';
    addButton.onclick = () => {
      this.map.removeLayer(this.marker);
      this.marker = null;
      this.openForm(e);
    };

    const cancelButton = document.createElement('button');
    cancelButton.innerHTML = 'Отмена';
    cancelButton.onclick = () => {
      this.map.removeLayer(this.marker);
      this.marker = null;
    };

    buttonContainer.append(addButton);
    buttonContainer.append(cancelButton);
    // Добавление кнопок над маркером
    this.marker.bindPopup(buttonContainer).openPopup();
  }
  openForm(e : any){
    this.marker = L.marker(e.latlng).addTo(this.map);
    const placeInfo = document.createElement('div');
placeInfo.className = "flex flex-col text-lg";

// Создание поля ввода для имени
const nameInput = document.createElement('input');
nameInput.id = "place-name"; // Уникальный id
nameInput.name = "name"; // Уникальный name
nameInput.placeholder = "Название места";
nameInput.autocomplete = "off"; // Добавление атрибута autocomplete

// Создание поля ввода для адреса
const addressInput = document.createElement('input');
addressInput.id = "place-address"; // Уникальный id
addressInput.name = "address"; // Уникальный name
addressInput.placeholder = "Адрес";
addressInput.autocomplete = "street-address"; // Добавление атрибута autocomplete

// Создание текстовой области для описания
const descInput = document.createElement('textarea');
descInput.id = "place-description"; // Уникальный id
descInput.name = "description"; // Уникальный name
descInput.placeholder = "Описание";
descInput.autocomplete = "off"; // Добавление атрибута autocomplete

// Создание кнопки подтверждения
const confirmButton = document.createElement('button');
confirmButton.id = "confirm-button"; // Уникальный id
confirmButton.name = "confirm"; // Уникальный name
confirmButton.innerHTML = 'Добавить';
confirmButton.type = "button"; // Убедитесь, что это кнопка, а не отправка формы
    confirmButton.onclick = () => {
      // Ensure `e` is passed correctly if it's used for coordinates
      let pl = new Place();
      pl.dateofcreation = new Date();
      // Assume `this` is bound correctly, otherwise pass `e` as a parameter to this function
      pl.latitude = e.latlng.lat;
      pl.longitude = e.latlng.lng;
      pl.adress = addressInput.value; // Corrected spelling from `adress`
      pl.name = nameInput.value;
      pl.description = descInput.value;
      this.addPlace(pl);
    };

    // Append elements to placeInfo div
    placeInfo.appendChild(nameInput);
    placeInfo.appendChild(addressInput);
    placeInfo.appendChild(descInput);
    placeInfo.appendChild(confirmButton); // Append the button

    // Bind the popup to the marker and open it
    this.marker.bindPopup(placeInfo).openPopup();

  }
  createRoute(route : L.LatLng[]): void {
    this.way = L.Routing.control({
      waypoints: route,
      routeWhileDragging: true,
      addWaypoints : true,
      router: L.Routing.osrmv1({
        serviceUrl: `https://router.project-osrm.org/route/v1`,
        language : 'ru',
        profile : 'foot',
      }),
      lineOptions: {
          styles: [{ color: 'blue', opacity: 1, weight: 5 }],
          extendToWaypoints: true,
        missingRouteTolerance: 10
      },
      show: false // Отключить отображение инструкций
    });
    this.way.addTo(this.map);
  }
  toCreateRoute(){
    this.finishCreate = !this.finishCreate;
  }
  addToRoute(place: Place) {
    console.log('Added to route:', place);
    this.creationRoutesPlace.push(place.id);
    this.map.closePopup();
  }

  routeStart(rutIndex : number){
    let waypoints  : Place[] = [];
    this.routeService.loadPlace(rutIndex).subscribe((res)=>{
        waypoints = res;
        this.getPosition().then((cord)=>{
         let temp = [L.latLng(cord.coords.latitude,cord.coords.longitude)];

        let toch :  L.LatLng[] = waypoints.map(a => new L.LatLng(a.latitude, a.longitude));
         temp.push(...toch);
          this.createRoute(temp);
          waypoints.forEach((elem) =>{
              this.addMarkers(elem)
          })
        }).catch(error =>{

        })
    })
  }
  routeFinish(){
    this.toast.success("Сасибо за прохождение маршрута <3!!");
    this.action = 'choose'
    this.map.removeControl(this.way);
    this.placesInRoute.forEach(elem => {
      this.map.removeLayer(elem);
    })
    this.placeService.getAllPlace().subscribe((res)=>{
      this.listPlaces = res;
      this.showPlaces();
    });
    this.feedBack = !this.feedBack;
    this.router.navigate([], {
      queryParams: {}
    });

  }
  cancelFeedback(){
    this.feedBack = !this.feedBack;
  }
  acceptFeedback(){
    this.fillFeedBack = !this.fillFeedBack;
  }
  sendFeedBack(){
    let feed : Feedback = new Feedback();
    feed.description = this.feedBackText;
    feed.routeid = this.rut;
    feed.score = this.score;
    // feed. а тут мы проверяем вошёл ли юзер но этого не будет никогда

    this.feedService.saveFeedBack(feed).subscribe(()=>{
      this.toast.success("Спасибо за отзыв")
      this.feedBack = !this.feedBack;
      this.fillFeedBack = !this.fillFeedBack;
    }, error =>{
      console.log(error);
      this.toast.error("Ошибка отправки отзыва")

    })
  }
  onRatingChange(newRating: number) {
    this.score = newRating;
  }
  centerOnMe(){
    this.getPosition().then( pos => {
      let center = { lat: pos.coords.latitude, lng: pos.coords.longitude};
      this.map.setView(center,16);
    })
    .catch(err => {
      console.error(err);
    });
  }
  addPlace(place : Place){
    console.log('Added to route:', place);
    this.placeService.savePlace(place).subscribe((res)=>{
      this.map.removeLayer(this.marker);
      this.marker = null;
      place.id = res;
      this.addMarkers(place);
      this.action = 'choose';
      this.map.on('click', (e) => {
        this.onMapClick(e);
      });
      this.toast.success("Место успешно добавлено")
    }, error =>{
      this.toast.error('Ошибка добавления');
      console.log(error);

    });


  }
  saveRoute(){
     this.map.on('click',(e) => {
      this.onMapClick(e);
    });

    let tm = new Route();
    tm.description = this.descNewRoute;
    tm.id = this.routeService.routes.length+1;
    tm.length =466;
    tm.paid = false;
    tm.name = this.nameNewRoute;
    this.routeService.saveRoute(tm).subscribe((res)=>{
      this.routeService.applyPlaceToRoute(res,this.creationRoutesPlace).subscribe(()=>{
        this.routeService.routes.push(tm);
        this.action = 'choose';
        this.toast.success("Маршрут сохранён");
        this.finishCreate = !this.finishCreate;
      }, error =>{
        console.log(error);
        this.toast.error("Ошибка добавления");
      })
    }, error =>{
      console.log(error);
      this.toast.error("Ошибка добавления");
    })
  }
  removeMarker(marker: any) {
    this.map.removeLayer(marker);
  }
  private addUserMarker(lat: number, lng: number): void {
    this.userMarker = L.marker([lat, lng]).setIcon(this.userIcon).addTo(this.map);
  }

  private updateUserMarker(lat: number, lng: number): void {
    if (this.userMarker) {
      this.userMarker.setLatLng([lat, lng]);
    }
  }
}
