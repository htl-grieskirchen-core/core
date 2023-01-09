import {Component, Input, OnInit} from "@angular/core";
import {Plugin} from "../../shared/classes/plugin";

@Component({
  selector: "app-category",
  templateUrl: "./category.component.html",
  styleUrls: ["./category.component.css"],
})
export class CategoryComponent implements OnInit {

  @Input() plugins: Plugin[] = [];
  @Input() title: string | null = "";
  @Input() icon: string | null = "";

  hidden: boolean = true;

  constructor() {}

  ngOnInit(): void {
    this.hidden = true;
  }
}
