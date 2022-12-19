import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home/home.component";
import { CategoryComponent } from "./category/category.component";
import { PluginComponent } from "./plugin/plugin.component";

@NgModule({
  declarations: [HomeComponent, CategoryComponent, PluginComponent],
  imports: [CommonModule, HomeRoutingModule],
})
export class HomeModule {}
