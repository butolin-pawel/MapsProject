import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Route } from '../class/route';
import { RouteService } from '../service/route.service';
import { Router, RouterModule } from '@angular/router';
import { RouteModalComponent } from '../route-modal/route-modal.component';

@Component({
  selector: 'app-routes',
  standalone: true,
  imports: [CommonModule,RouterModule,RouteModalComponent],
  templateUrl: './routes.component.html',
  styleUrl: './routes.component.css'
})
export class RoutesComponent {
  list! : Route[];
  isModal : boolean = false;
  modalRoute! : Route;
  constructor(private routeService : RouteService,private router : Router){
    routeService.getAllRoutes().subscribe((res)=>{
      this.list = res;
    });
  }
  routeInfo(route : Route){
    this.modalRoute = route;
    this.isModal = !this.isModal
  }
  closeModal(){
    this.isModal = !this.isModal;
    this.modalRoute = new Route();
  }
}
