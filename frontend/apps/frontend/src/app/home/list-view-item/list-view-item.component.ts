import {Component, Input, OnInit} from "@angular/core";
import {Plugin} from "../../shared/classes/plugin";

@Component({
  // eslint-disable-next-line  @angular-eslint/component-selector
  selector: "tr[app-list-view-item]",
  templateUrl: "./list-view-item.component.html",
  styleUrls: ["./list-view-item.component.css"],
})
export class ListViewItemComponent implements OnInit {

  @Input() plugin!: Plugin;

  constructor() {}

  ngOnInit(): void {
    console.log('')
  }
}
