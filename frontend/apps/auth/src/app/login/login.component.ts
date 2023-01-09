import {Component} from "@angular/core";
import {MainframeService} from "../mainframe.service";
import {CredService} from "../core/cred.service";

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

  constructor(private mainframe: MainframeService, private credService: CredService) {
  }

  async login() {
    if (this.loading)
      return;

    this.loading = true;
    this.error = null;

    const pwEncrypted = await this.credService.encryptPassword(this.password);

    this.mainframe.login(this.username, this.password).then(error => {
      this.loading = false;
      this.error = error;
    })
  }
}
