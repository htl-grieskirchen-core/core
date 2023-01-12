import {Component, OnInit} from "@angular/core";
import {HttpClient} from '@angular/common/http';

@Component({
  selector: "app-daily-quote",
  templateUrl: "./daily-quote.component.html",
  styleUrls: ["./daily-quote.component.css"],
})
export class DailyQuoteComponent implements OnInit {
  constructor(private http: HttpClient) {
  }

  configUrl = 'https://localhost:7022/Quotes';
  Quote: string | null = null;
  SubmittedBy: string | null = null;
  QuoteID: string | null = null;
  QuoteDate: string | null = null;
  showImage = false;

  getConfig() {
    return this.http.get<any>(this.configUrl);
  }

  ngOnInit(): void {

    let currYear = new Date().getFullYear();

    let images = [
      {start: new Date(currYear, 11, 21), end: new Date(currYear, 11, 26),
        name: 'XMAS', img: "https://github.com/htl-grieskirchen-core/core-assets/blob/develop/img/lars.jpeg?raw=true", subtxt: "Frohe Weihnachten!"},
      {start: new Date(currYear, 12, 31), end: new Date(currYear, 0, 2),
        name: 'NEWYEAR', img: "https://github.com/htl-grieskirchen-core/core-assets/blob/develop/img/f8gqc.gif?raw=true", subtxt: "Frohes neues Jahr!"}
    ]

    let search = images.find(i => new Date() > i.start && new Date() < i.end);
    if(search) {
      this.Quote = search.img;
      this.SubmittedBy = search.subtxt;
      this.QuoteID = search.name;
      this.showImage = true;
    } else {
      this.getConfig().subscribe((data) => {
        this.Quote = data['quoteText'];
        this.SubmittedBy = data['submittedBy'];
        this.QuoteID = data['quoteId'];
        this.QuoteDate = data['submitTime'];
        console.log(data)
      });
    }
  }
}
