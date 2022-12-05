import { Component } from "@angular/core";
import { MainframeService } from "../mainframe.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent {
  username = "";
  password = "";

  loading = false;
  error: null | string = null;

  constructor(private mainframe: MainframeService) {}

  login() {
    if(this.loading)
      return;

    this.loading = true;
    this.error = null;

    this.mainframe.login(this.username, this.password).then(error => {
      this.loading = false;
      this.error = error;
    })
  }
}
