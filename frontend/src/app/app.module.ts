import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {NgxCoreUiModule} from "../../projects/ngx-core-ui/src/lib/ngx-core-ui.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    NgxCoreUiModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
