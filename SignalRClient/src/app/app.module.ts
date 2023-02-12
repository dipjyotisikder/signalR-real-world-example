import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { SharedModule } from './shared/shared.module';
import { ServerlessComponent } from './serverless/serverless.component';
import { SelfhostedComponent } from './selfhosted/selfhosted.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutes,
    SharedModule,
    FormsModule,
    HttpClientModule,
  ],

  declarations: [AppComponent, ServerlessComponent, SelfhostedComponent],

  bootstrap: [AppComponent],
})
export class AppModule {}
