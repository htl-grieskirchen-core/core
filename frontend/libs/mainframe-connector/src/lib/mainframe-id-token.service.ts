import {Injectable} from '@angular/core';
import jwtDecode from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class MainframeIdTokenService {

  constructor() {
  }

  public getJwt(): Promise<string> {
    parent.postMessage({
      method: "getIDToken",
      pluginId: "navigation",
    }, "*");

    const promise = new Promise<string>((resolve) => {
      window.addEventListener("message", (event) => {
        if (event.data.method === "getIDToken") {
          resolve(event.data.data);
        }
      });
    });

    return promise;
    //return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0eXBlIjoiaWQtdG9rZW4iLCJ1c2VybmFtZSI6InNmZWljaHRsYmF1ZXIxOCIsInV1aWQiOiJzdXBhIHV1aWQgaG9zdCBkdSBkbyIsInJvbGxlIjoic3R1ZGVudCIsImVtYWlsIjoic2ZlaWNodGxiYXVlcjE4QHN1cy5odGwtZ3JpZXNraXJjaGVuLmF0IiwiY29ubmVjdGVkUGxhdGZvcm1zIjoiW1wiZmVpY2h0bGJhdWVyLmRkbnMubmV0XCIsXCJzcGllbGFmZmUuZGVcIixcInNjaG51cGZ0YWJha2JpbGxpZ2VyMjQuYXRcIl0iLCJtYXRyaWtlbG51bW1lciI6IjY5Iiwia2xhc3NlIjoiNUIifQ._VJ_BC8x6GTTMbQ_ZBKD84UArUWdaSmGOTKj8SghEFM";
  }

  public decodeJwt(token: string): IDTokenPayload {
    const payload = jwtDecode<IDTokenPayload>(token);
    payload.connectedPlatformsDeserialized = JSON.parse(payload.connectedPlatforms);
    return payload;
  }

}

// modelled after the Claims Property of class IDToken in the Core.Backend.Secure project
export interface IDTokenPayload {
  // type: "id-token"
  type: string,

  // username: ldap-username
  username: string,

  // uid: db-uuid
  uuid: string,

  // rolle: <schüler, lehrer, staff>
  rolle: string,

  // Email
  email: string,

  // connectedPlatforms: serialisiertes Json-Array -- Plattformen mit hinterlegten credentials
  connectedPlatforms: string,

  // connectedPlatformsDeserialized: connectedPlatforms, oba deserialisiert
  connectedPlatformsDeserialized: string[],

  // matrikelnummer - nur bei Schülern verfügbar
  matrikelnummer: string | undefined,

  // klasse - nur bei Schülern verfügbar
  klasse: string | undefined,
}
