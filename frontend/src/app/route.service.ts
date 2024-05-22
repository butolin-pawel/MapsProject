import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  route : L.LatLng[] = []
  constructor() { }
  addPoint(latlng : any){
    this.route.push(latlng);
  }
  getRoute(){
    return this.route;
  }
}
