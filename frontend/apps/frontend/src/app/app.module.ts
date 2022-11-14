import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";

import {AppComponent} from "./app.component";
import {MainframeConnectorModule} from "mainframe-connector";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    MainframeConnectorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
