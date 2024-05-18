import { Component } from '@angular/core';
import {faSearch} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [FontAwesomeModule,CommonModule,RouterModule],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
lupaIcon = faSearch;
page : number = 1;
constructor(){

}
pageChange(pg : number){
  this.page = pg;
}
}
