import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {LoadHandlerService} from "./load-handler-service";


@inject(HttpFetch, LoadHandlerService)
export class HotelRulesService {
  data;

  constructor(private httpFetch: HttpFetch, private loadHandlerService: LoadHandlerService) {
  }

  getRules() {
    this.loadHandlerService.setBusy();

    let promise = new Promise((resolve, reject) => {
      if (!this.data) {
        this.httpFetch.fetch('/api/Home')
          .then(response => response.json())
          .then(data => {
            this.data = data;
            resolve(data);
          })
          .catch(err => reject(err))
          .then(() => this.loadHandlerService.setFree());
      }
      else
        resolve(this.data);
    });
    return promise;
  }

  getRule(ruleID) {
    this.loadHandlerService.setBusy();

    let promise = new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Home/' + String(ruleID))
        .then(response => response.json())
        .then(data => {
          resolve(data);
        })
        .catch(err => reject(err))
        .then(() => this.loadHandlerService.setFree());
    });
    return promise;
  }
}
