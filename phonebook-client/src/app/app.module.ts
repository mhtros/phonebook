import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import {
  BrowserAnimationsModule,
  NoopAnimationsModule,
} from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { MainComponent } from './components/main/main.component';
import { ManagePhonebookComponent } from './components/sub/manage-phonebook/manage-phonebook.component';
import { AngularMaterialModule } from './modules/angular-material/angular-material/angular-material.module';
import { ErrorInterceptorService } from './services/error-interceptor.service';
import { InterceptorService } from './services/interceptor.service';

@NgModule({
  declarations: [AppComponent, MainComponent, ManagePhonebookComponent],
  imports: [
    FormsModule,
    BrowserModule,
    HttpClientModule,
    NoopAnimationsModule,
    AngularMaterialModule,
    BrowserAnimationsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptorService,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
