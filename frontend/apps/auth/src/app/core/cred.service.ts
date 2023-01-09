import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CredService {

  publicKey = "-----BEGIN PUBLIC KEY-----" +
    "MIIBojANBgkqhkiG9w0BAQEFAAOCAY8AMIIBigKCAYEAqet17YQsSly8QxXC6S3W" +
    "8BL+dmSBZmU15OFyycmEQiq5KmgvBZpD8qyzas8iel/w9jdaefsCNE+1tVaPYD9w" +
    "PQzn3Eyzq1P4a87UC4ypcQtJoJQHck8EpRy+VCnA+0iVSjGNqeDK8CjPWz7WyVWg" +
    "/tVXmHYLhcGLUft2sb9Ry2VtntrPGNORMGYXXkuErh242oOphiRV4tXnAfzQaRRm" +
    "Oi1gUxb2I/HWe8tXJ+Teg7oBb+Lg04yzMh8o+wGXpmhtNAZGvn9Z4RYcmjbStNkP" +
    "BESEMmDu8z252kWBiJboeq48QMOMWOQnWdnh+anj9XQlM7yhzYL9yR3PwXtLwNQC" +
    "qSEnjYRIWrGQyIEYps3jDZDe5MHZPRFOypcycD5A2pvn52UZOXsbVnz6bMipbxmv" +
    "XOw0qZZPzSGTa9n/FFNrI5W0XhqvxXjbRTMWLSWwfEjjy4ztEqA/8YbRdsfiRXva" +
    "10gIc9/GzQZFpnRHYeZ+PpQUbRG2scUygwuGJNYXS045AgMBAAE=" +
    "-----END PUBLIC KEY-----";

  // https://stackoverflow.com/a/62967202/11864516

  async encryptPassword(pwPlain: string): Promise<string> {
    try {
      const pub = await this.importPublicKey(this.publicKey);
      const encrypted = await this.encryptRSA(pub, new TextEncoder().encode(pwPlain));
      const encryptedBase64 = window.btoa(this.ab2str(encrypted));
      console.log(encryptedBase64.replace(/(.{64})/g, "$1\n"));
      return encryptedBase64;
    } catch (error) {
      console.log(error);
      return "";
    }
  }

  async importPublicKey(spkiPem: string) {
    return await window.crypto.subtle.importKey(
      "spki",
      this.getSpkiDer(spkiPem),
      {
        name: "RSA-OAEP",
        hash: "SHA-512",
      },
      true,
      ["encrypt"]
    );
  }

  async encryptRSA(key: CryptoKey, plaintext: ArrayBuffer) {
    return await window.crypto.subtle.encrypt(
      {
        name: "RSA-OAEP"
      },
      key,
      plaintext
    );
  }

  getSpkiDer(spkiPem: string) {
    const pemHeader = "-----BEGIN PUBLIC KEY-----";
    const pemFooter = "-----END PUBLIC KEY-----";
    const pemContents = spkiPem.substring(pemHeader.length, spkiPem.length - pemFooter.length);
    const binaryDerString = window.atob(pemContents);
    return this.str2ab(binaryDerString);
  }

  str2ab(str: string) {
    const buf = new ArrayBuffer(str.length);
    const bufView = new Uint8Array(buf);
    for (let i = 0, strLen = str.length; i < strLen; i++) {
      bufView[i] = str.charCodeAt(i);
    }
    return buf;
  }

  ab2str(buf: Iterable<number>) {
    return String.fromCharCode.apply(null, Array.from(new Uint8Array(buf)));
  }
}
