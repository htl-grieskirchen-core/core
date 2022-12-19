import {Component, HostBinding, Input, OnInit} from "@angular/core";

@Component({
  selector: "app-plugin",
  templateUrl: "./plugin.component.html",
  styleUrls: ["./plugin.component.scss"],
})
export class PluginComponent implements OnInit {

  background: string = "";

  @Input() set imageSrc (src: string | null) {
    this.background = `background-image: linear-gradient(rgba(0,0,0, 0),rgba(0,0,0,0.8)), url(${src})`;
  };
  @Input() name: string | null = "";

  constructor() {}

  ngOnInit(): void {
    console.log('test')
  }
}
