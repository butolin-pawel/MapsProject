import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';
@Component({
  selector: 'app-starrating',
  standalone: true,
  imports: [FontAwesomeModule,CommonModule],
  templateUrl: './star-rating.component.html',
  styleUrl: './star-rating.component.css'
})
export class StarRatingComponent {
  @Input() maxRating: number = 5;
  @Input() initialRating: number = 1;
  @Input() interactive: boolean = true;

  @Output() ratingChange = new EventEmitter<number>();

  rating: number = this.initialRating;
  ngOnInit(){
    this.stars = Array(this.maxRating).fill(0).map((x, i) => i + 1);
    this.rating = this.initialRating;
  }
  stars: number[] = []
  starIcon = faStar;
  selectRating(rating: number) {
    if (this.interactive) {
      this.rating = rating;
      this.ratingChange.emit(this.rating);
    }
  }
}
