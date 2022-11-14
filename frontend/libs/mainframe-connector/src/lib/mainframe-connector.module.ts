import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {AngularResizeEventModule} from "angular-resize-event";
import {PluginPlaceholderComponent} from "./plugin-placeholder/plugin-placeholder.component";

@NgModule({
  declarations: [
    PluginPlaceholderComponent
  ],
  imports: [
    CommonModule,
    AngularResizeEventModule
  ],
  exports: [
    PluginPlaceholderComponent
  ]
})
export class MainframeConnectorModule {
}
