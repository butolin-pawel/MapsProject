import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Feedback } from '../class/feedback';
import { backUrl } from '../../main';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  constructor(private http : HttpClient) {

  }

  saveFeedBack(feed : Feedback){
      return this.http.post(backUrl+"Feedback",feed);
  }
  getAllByRoute(routeId : number){
    return this.http.get<Feedback[]>(backUrl+"Feedback/"+routeId);
  }
}
