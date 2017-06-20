import {inject} from 'aurelia-framework';
import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {LoadHandlerService} from "./load-handler-service";


@inject(HttpFetch, LoadHandlerService)
export class CasesService {
  technicanCases: {}[];
  cleanerCases: {}[];

  constructor(private httpFetch: HttpFetch, private loadHandlerService: LoadHandlerService) {
    this.technicanCases = [];
    this.cleanerCases = [];
  }

  // 2. Taski zgłoszone przez klienta,
  // gdzieś na panelu klienta korzystając z metody GET z url
  // /api/Tasks/CustomerTasks
  // dostaniecie okrojoną wersję tasków,
  // wyświetlcie je w jakiejś liście pod nowym przyciskiem może
  // (Zgłoszone sprawy?),
  // Wyswietlcie tylko Description i Status

  getCustomersTasks() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Tasks/CustomerTasks')
        .then(response => console.log('response', response))
        .catch(err => (reject(err)))
        .then(() => this.loadHandlerService.setFree());
    });
  }

  getTechnicianCases() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      if (this.technicanCases.length < 1) {
        this.httpFetch.fetch('/api/Case/Filter/Technician')
          .then(response => response.json())
          .then(data => {
            let tmp = JSON.parse(JSON.stringify(data));
            this.technicanCases = tmp.cases;
            resolve(this.technicanCases);
          })
          .catch(err => reject(err))
          .then(() => this.loadHandlerService.setFree());
      } else {
        this.loadHandlerService.setFree();
        resolve(this.technicanCases);
      }

    });
  }

  getCleanerCases() {
    this.loadHandlerService.setBusy();

    return new Promise((resolve, reject) => {
      if (this.cleanerCases.length < 1) {
        this.httpFetch.fetch('/api/Case/Filter/Cleaner')
          .then(response => response.json())
          .then(data => {
            let tmp = JSON.parse(JSON.stringify(data));
            this.cleanerCases = tmp.cases;
            resolve(this.cleanerCases);
          })
          .catch(err => reject(err))
          .then(() => this.loadHandlerService.setFree());
      } else {
        this.loadHandlerService.setFree();
        resolve(this.cleanerCases);
      }

    });
  }



}
