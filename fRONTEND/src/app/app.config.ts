import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { customInterceptor } from './interceptor/custom.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([customInterceptor])),
    provideRouter(routes),
    importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot({
        positionClass: 'toast-top-right',   // Slide in from top-right
        timeOut: 3000,                      // Auto-dismiss after 3s
        progressBar: true,                  // Show progress
        tapToDismiss: true,                // Tap to close
        newestOnTop: true,                 // New toast stacks on top
        easeTime: 300,                     // Smooth in/out
        extendedTimeOut: 1000              // How long it stays on hover
      })
      
    )
  ]
};
