import { Routes } from '@angular/router';
import { MapComponent } from './map/map.component';
import { RoutesComponent } from './routes/routes.component';
import { LoginComponent } from './login/login.component';

export const routes: Routes = [
  {path:"",component:MapComponent},
  {path:"routes", component:RoutesComponent},
  {path:"login", component:LoginComponent}
];
