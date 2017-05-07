import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class CasesService {
  cases: {}[];

  constructor(private httpFetch: HttpFetch) {
  }

  getCases() {
    let promise = new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Case')
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(err => reject(err));
    });
    return promise;
  }
}
