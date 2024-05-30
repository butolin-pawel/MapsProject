import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Route } from '../class/route';
import { RouteService } from '../service/route.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-routes',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './routes.component.html',
  styleUrl: './routes.component.css'
})
export class RoutesComponent {
  list! : Route[];

  constructor(private routeService : RouteService,private router : Router){
    routeService.getAllRoutes().subscribe((res)=>{
      this.list = res;
    });
  }

}
