import { Injectable } from '@angular/core';
import { Route } from '../class/route';
import { HttpClient } from '@angular/common/http';
import { backUrl } from '../../main';
import { Place } from '../class/place';

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  routes : Route[] = []
  constructor(private http : HttpClient) { }

  getRoute(i : number){
    return this.routes[i];
  }
  getAllRoutes(){
    return this.http.get<Route[]>(backUrl+"Route");
  }
  saveRoute(r : Route){
    return this.http.post<number>(backUrl+"Route",r);
  }
  applyPlaceToRoute(rId : number, placesId : number[]){
    return this.http.post(backUrl+"RoutePlace/"+rId,placesId);
  }
  loadPlace(rId : number){
    return this.http.get<Place[]>(backUrl+"RoutePlace/"+rId);
  }
}
