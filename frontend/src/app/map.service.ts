import { Injectable } from '@angular/core';
import * as L from 'leaflet';
@Injectable({
  providedIn: 'root'
})
export class MapService {
  private map!: L.Map;

  constructor() { }

  initializeMap(mapElement: any, center: L.LatLngLiteral, zoom: number): void {
    this.map = L.map(mapElement).setView(center, zoom);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);
  }

  addMarker(lat: number, lng: number): void {
    L.marker([lat, lng]).addTo(this.map);
  }

  createRoute(points: L.LatLngLiteral[]): void {
    L.polyline(points, { color: 'blue' }).addTo(this.map);
  }
}
