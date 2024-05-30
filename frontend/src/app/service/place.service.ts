import { Injectable } from '@angular/core';
import { Place } from '../class/place';
import { HttpClient } from '@angular/common/http';
import { backUrl } from '../../main';

@Injectable({
  providedIn: 'root'
})
export class PlaceService {
  constructor(private http : HttpClient) {

   }
   getAllPlace(){
    return this.http.get<Place[]>(backUrl+"Place");
   }
  savePlace(p : Place){
    return this.http.post<any>(backUrl+"Place",p);
  }
}
