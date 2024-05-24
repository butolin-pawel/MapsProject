import { Component, ElementRef, ViewChild } from '@angular/core';
import { MapService } from '../map.service';
import * as L from 'leaflet';
import { RouteService } from '../route.service';
import 'leaflet-routing-machine';
import { BrowserModule } from '@angular/platform-browser';
import { delay } from 'rxjs';
import { faLocationArrow } from '@fortawesome/free-solid-svg-icons/faLocationArrow';
import { ToastrModule, ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-map',
  standalone: true,
  imports: [ ToastrModule],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent {
  @ViewChild('map') mapElement!: ElementRef;

  constructor(private mapService: MapService, private routeService : RouteService, private toast : ToastrService) { }
  private map!: L.Map;
  private userMarker!: L.Marker;
  marker: any;
  watchId! : number;
  way : any;
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
  }
  ngOnDestroy(): void {
    // Остановка отслеживания позиции при уничтожении компонента
    if (this.watchId) {
      navigator.geolocation.clearWatch(this.watchId);
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

      this.map.on('click', (e) => {
        this.onMapClick(e);
      });

      this.addUserMarker(pos.coords.latitude,pos.coords.longitude);
    })
    .catch(err => {
      console.error(err);
    });

  }

  addMarkers(lat : number, lng : number, comm : string): void {
    const descriptionPoint =  document.createElement('p');
    descriptionPoint.append(comm);
    descriptionPoint.className = "text-lg text-wrap"
    L.marker([lat, lng]).bindPopup(descriptionPoint).addTo(this.map);
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
    addButton.innerHTML = 'Добавить в маршрут';
    addButton.onclick = () => {
      this.addToRoute(e.latlng);
    };

    const cancelButton = document.createElement('button');
    cancelButton.innerHTML = 'Отмена';
    cancelButton.onclick = () => {
      this.map.removeLayer(this.marker);
    };

    buttonContainer.append(addButton);
    buttonContainer.append(cancelButton);
    // Добавление кнопок над маркером
    this.marker.bindPopup(buttonContainer).openPopup();
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

  addToRoute(latlng: any) {
    this.map.removeLayer(this.marker);
    this.marker = null;
    console.log('Added to route:', latlng);
    this.routeService.addPoint(latlng)
  }

  routeStart(){
    let route = this.routeService.getRoute();
    this.getPosition().then((cord)=>{
     let temp = [L.latLng(cord.coords.latitude,cord.coords.longitude)];
     temp.push(...route);
      this.createRoute(temp);
      route.forEach((elem, index) =>{
          this.addMarkers(elem.lat,elem.lng, 'Точка ' + (index+1))
      })
    }).catch(error =>{

    })
  }
  routeFinish(){
    this.toast.success("Сасибо за прохождение маршрута")
    this.map.removeLayer(this.way);
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
