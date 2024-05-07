import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import {ɵBrowserAnimationBuilder} from '@angular/animations'
import {ToastrModule, provideToastr} from 'ngx-toastr'
import {provideAnimations} from '@angular/platform-browser/animations'
export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),provideHttpClient(withFetch()),provideToastr(),CommonModule,BrowserModule,HttpClientModule,ɵBrowserAnimationBuilder,provideAnimations()]
};


