import {Component, OnInit} from "@angular/core";
import {ResizedEvent} from "angular-resize-event";
import {MainframeNavService} from "../mainframe-nav.service";

@Component({
  selector: "mainframe-plugin-placeholder",
  templateUrl: "./plugin-placeholder.component.html",
  styleUrls: ["./plugin-placeholder.component.css"]
})
export class PluginPlaceholderComponent {
  constructor(private mainframeNavService: MainframeNavService) {
  }

  sendPluginResized(event: ResizedEvent) {
    let rect = event.newRect;
    this.mainframeNavService.resizePlugin(rect.left, rect.top, rect.width, rect.height);
  }
}
