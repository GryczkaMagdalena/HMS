import {inject} from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {CasesService} from '../../../../services/cases-service';


@inject(CasesService, Router)
export class RoomSupport {
  cases: {}[];
  selectedCase: {};

  constructor(private casesService: CasesService, private router: Router) {
    casesService.getCleanerCases().then(res => {
      let tmpCases = JSON.parse(JSON.stringify(res));
      this.cases = tmpCases;
    });
  }

  selectCase(selectedCase) {
    this.selectedCase = selectedCase;
    console.log('this.selectedCase ', this.selectedCase);
  }

  returnToMainPanel(){
    this.router.navigateToRoute('base');
  }
}
