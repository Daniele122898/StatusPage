import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatDividerModule} from '@angular/material/divider';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import {RouterModule} from '@angular/router';
import {appRoutes} from './routes';
import { StatusContainerComponent } from './home/components/status-container/status-container.component';
import {HttpClientModule} from '@angular/common/http';
import { AdminDashboardComponent } from './admin-dashboard/pages/admin-dashboard/admin-dashboard.component';
import { LoginComponent } from './admin-dashboard/pages/login/login.component';
import {JwtModule} from '@auth0/angular-jwt';

export function tokenGetter(): string {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    StatusContainerComponent,
    AdminDashboardComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatExpansionModule,
    MatDividerModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        // TODO make this better
        allowedDomains: ['localhost:5000/api'],
        disallowedRoutes: ['localhost:5000/api/auth']
      }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
