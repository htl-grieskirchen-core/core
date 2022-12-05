import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class MainframeService {

  constructor() {
  }

  login(username: string, password: string): Promise<string> {
    const promise = new Promise<string>((resolve) => {
      window.addEventListener("message", (event) => {
        if (event.data.method === "error") {
          resolve(event.data.data.message);
        }
      });
    });

    window.parent.postMessage({
      method: "login",
      data: {
        username: username,
        password: password
      }
      // FIXME: Fix targetOrigin
    }, "*");

    return promise;
  }
}
