import {Injectable} from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class MainframeNavService {
  constructor() {
  }

  public openPlugin(pluginId: string) {
    console.debug("Sending plugin open message: ", pluginId);
    window.parent.postMessage({method: "loadPlugin", plugin: pluginId});
  }

  public resizePlugin(left: number, top: number, width: number, height: number) {
    console.debug("Sending plugin resize message: ", left, top, width, height);
    window.parent.postMessage({
      method: "container",
      x: left,
      y: top,
      width,
      height
    });
  }
}
