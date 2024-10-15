import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { BsModalService } from 'ngx-bootstrap/modal';


import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [BsModalService, provideHttpClient(), provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes)]
};
