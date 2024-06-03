import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Route } from '../class/route';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { CommonModule } from '@angular/common';
import { StarRatingComponent } from '../star-rating/star-rating.component';
import { Feedback } from '../class/feedback';
import { FeedbackService } from '../service/feedback.service';
@Component({
  selector: 'app-route-modal',
  standalone: true,
  imports: [FontAwesomeModule,CommonModule,StarRatingComponent],
  templateUrl: './route-modal.component.html',
  styleUrl: './route-modal.component.css'
})
export class RouteModalComponent {
  @Input() route : Route = new Route();

  @Output() closeModal = new EventEmitter<number>();
  feedBacks : Feedback[]= [];
  close = faTimes;
  constructor(private feedService : FeedbackService){

  }
  ngOnChanges(){
      this.feedService.getAllByRoute(this.route.id).subscribe((res)=>{
        this.feedBacks = res;
      })
  }
  closeEmit(){
    this.closeModal.emit();
  }
}
