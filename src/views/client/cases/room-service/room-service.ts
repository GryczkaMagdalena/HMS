import {inject} from 'aurelia-framework';
import {CasesService} from '../../../../services/cases-service';


@inject(CasesService)
export class RoomService {
  cases: {}[];
  selectedCase: {};

  constructor(private casesService: CasesService) {
    casesService.getCleanerCases().then(res => {
      let tmpCases = JSON.parse(JSON.stringify(res));
      this.cases = tmpCases;
      console.log('this.cases', this.cases);
    });
  }

  selectCase(selectedCase) {
    this.selectedCase = selectedCase;
    console.log('this.selectedCase ', this.selectedCase);
  }
}
