import {Component, Input, OnInit} from "@angular/core";
import {Plugin} from "../../shared/classes/plugin";

@Component({
  selector: "app-category",
  templateUrl: "./category.component.html",
  styleUrls: ["./category.component.css"],
})
export class CategoryComponent implements OnInit {

  @Input() title: string | null = "";
  @Input() icon: string | null = "";

  hidden: boolean = true;

  // getNewPlugin = (name: string, image: string) => new Plugin( name, [], null, null, null, null, `../../../assets/images/${image}`);

  plugins: Plugin[] = [
    // this.getNewPlugin('BeReal Anfängerkurs', 'nodla.jpeg'),
    // this.getNewPlugin('Kirtag in Lambrechten', 'wiesn.jpg'),
    // this.getNewPlugin('Beamer hi mochn', 'heimooo.jpg'),
    // this.getNewPlugin('Zugbremse reparieren', 'BeReal.jpeg'),
    // this.getNewPlugin('Roboter-Quartett mit Sperrer', 'loata.jpeg'),
    // this.getNewPlugin('Schuhkressenbauer', 'schuhkressenbauer.jpg'),
    // this.getNewPlugin('Minecraft Modpacks', 'simon.jpeg'),
    // this.getNewPlugin('MIchael?', 'miche2.JPG'),
    // this.getNewPlugin('Chayacheck', 'groans.JPG'),
    // this.getNewPlugin('periodischer Chill', 'chille.jpg'),
    // this.getNewPlugin('ORF interview', 'meeting.jpg')
  ];

  constructor() {}

  ngOnInit(): void {
    console.log('test')
  }
}
