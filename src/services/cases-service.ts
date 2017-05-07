import {HttpClient as HttpFetch} from 'aurelia-fetch-client';
import {inject} from 'aurelia-framework';


@inject(HttpFetch)
export class CasesService {
  cases: {}[];

  constructor(private httpFetch: HttpFetch) {
    this.cases = [
      {
        caseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273801",
        title:"Wymiana żarówki",
        description:'Wymiana żarówki',
        workerType:"Technician"
      },
      {
        caseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273802",
        title:"Naprawa łóżka",
        description:'Naprawa łóżka',
        workerType:"Technician"
      },
      {
        caseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273803",
        title:"Naprawa telewizora",
        description:'Naprawa telewizora',
        workerType:"Technician"
      },
      {
        caseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273804",
        title:"Inne",
        description:'Inne',
        workerType:"Technician"
      },
      {
        caseID:"4ba83f3c-4ea4-4da4-9c06-e986a8273805",
        title:"ExampleCase",
        description:"Clean something",
        workerType:"Technician"
      }
    ]
  }

  getTechnicianCases() {
    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Case')
        .then(response => response.json())
        .then(data => resolve(data))
        .catch(err => reject(err));
    });
    // return new Promise((resolve, reject) => {
    //   resolve(this.cases);
    // });
  }

  getCleanerCases() {
    return new Promise((resolve, reject) => {
      this.httpFetch.fetch('/api/Case')
        .then(response => response.json())
        .then(data => resolve(data))
        .catch(err => reject(err));
    });
  }
}
