import {Component, Input, OnInit} from "@angular/core";
import {Plugin} from "../../shared/classes/plugin";

@Component({
  selector: "app-list-view",
  templateUrl: "./list-view.component.html",
  styleUrls: ["./list-view.component.css"],
})
export class ListViewComponent implements OnInit {

  _plugins: Plugin[] = [];
  @Input() set plugins(plugins: Plugin[]) {
    this._plugins = plugins;
    this.sortPlugins();
  };

  titleAsc: boolean = true;

  constructor() {}

  ngOnInit(): void {
    console.log('')
  }

  sortPlugins() {
    this._plugins = this._plugins.sort((a,b) =>
      this.titleAsc ? a.name!.localeCompare(b.name!) : b.name!.localeCompare(a.name!));
    console.log(this._plugins)
  }
}
