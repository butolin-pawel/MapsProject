import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-routes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './routes.component.html',
  styleUrl: './routes.component.css'
})
export class RoutesComponent {
  list : any[] =  [ {"name":"route1","paid":true,"during":2432},
  {"name":"route2","paid":false,"during":2432},
  {"name":"route3","paid":true,"during":2432},
  {"name":"route4","paid":false,"during":2432},
  {"name":"route5","paid":true,"during":2432},
  {"name":"route6","paid":false,"during":2562},]

}
