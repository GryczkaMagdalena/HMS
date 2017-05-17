import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class CasesService {
  technicanCases: {}[];
  cleanerCases: {}[];

  constructor(private httpFetch: HttpFetch) {
    this.technicanCases = [];
    this.cleanerCases = [];
  }

  getTechnicianCases() {
    return new Promise((resolve, reject) => {
      if (this.technicanCases.length < 1) {
        this.httpFetch.fetch('/api/Case/Filter/Technician')
          .then(response => response.json())
          .then(data => {
            let tmp = JSON.parse(JSON.stringify(data));
            this.technicanCases = tmp.cases;
            resolve(this.technicanCases);
          })
          .catch(err => reject(err));
      } else {
        resolve(this.technicanCases);
      }

    });
  }

  getCleanerCases() {
    return new Promise((resolve, reject) => {
      if (this.cleanerCases.length < 1) {
        this.httpFetch.fetch('/api/Case/Filter/Cleaner')
          .then(response => response.json())
          .then(data => {
            let tmp = JSON.parse(JSON.stringify(data));
            this.cleanerCases = tmp.cases;
            resolve(this.cleanerCases);
          })
          .catch(err => reject(err));
      } else {
        resolve(this.cleanerCases);
      }

    });
  }
}
