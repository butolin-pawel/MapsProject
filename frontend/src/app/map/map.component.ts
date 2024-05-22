import { Component, ElementRef, ViewChild } from '@angular/core';
import { MapService } from '../map.service';
import * as L from 'leaflet';
import { RouteService } from '../route.service';
import 'leaflet-routing-machine';
@Component({
  selector: 'app-map',
  standalone: true,
  imports: [],
  templateUrl: './map.component.html',
  styleUrl: './map.component.css'
})
export class MapComponent {
  @ViewChild('map') mapElement!: ElementRef;

  constructor(private mapService: MapService, private routeService : RouteService) { }
  private map!: L.Map;
  marker: any;
  ngOnInit(): void {
  }
  ngAfterViewInit(){

    this.initializeMap();
  }
  initializeMap(): void {
    const center: L.LatLngLiteral = { lat: 58.6035, lng: 49.666}; // Лондон, например
    const zoom = 16;
    this.map = L.map(this.mapElement.nativeElement).setView(center, zoom);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',{
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);
    this.map.on('click', (e) => {
      this.onMapClick(e);
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
    L.Routing.control({
      waypoints: route,
      routeWhileDragging: true,
      router: L.Routing.osrmv1({
        serviceUrl: `https://router.project-osrm.org/route/v1`,
        profile : "foot",
        language : 'ru'
      }),


    }).addTo(this.map);
  }
  addToRoute(latlng: any) {
    this.map.removeLayer(this.marker);
    this.marker = null;
    // Здесь реализуйте логику добавления точки в маршрут
    console.log('Added to route:', latlng);
    this.routeService.addPoint(latlng)
    // Например, добавление координаты в массив точек маршрута
  }
  routeStart(){
    let route = this.routeService.getRoute();
    // route.forEach((elem,index)=>
    // {this.addMarkers(elem.lat,elem.lng,"Это точка номер " + index)}
    // );
    this.createRoute(route);
  }
  removeMarker(marker: any) {
    this.map.removeLayer(marker);
  }
}
