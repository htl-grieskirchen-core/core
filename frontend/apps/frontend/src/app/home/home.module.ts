import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home/home.component";
import { CategoryComponent } from "./category/category.component";
import { PluginComponent } from "./plugin/plugin.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { ListViewComponent } from "./list-view/list-view.component";
import { ListViewItemComponent } from "./list-view-item/list-view-item.component";

@NgModule({
  declarations: [
    HomeComponent,
    CategoryComponent,
    PluginComponent,
    DashboardComponent,
    ListViewComponent,
    ListViewItemComponent,
  ],
  imports: [CommonModule, HomeRoutingModule],
})
export class HomeModule {}
