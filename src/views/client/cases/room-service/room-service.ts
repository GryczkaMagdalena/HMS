import {inject} from 'aurelia-framework';
import {CasesService} from '../../../../services/cases-service';


@inject(CasesService)
export class RoomService {
  cases: {}[];
  selectedId: number;

  constructor(private casesService: CasesService) {
    casesService.getCleanerCases().then(res => {
      this.cases = JSON.parse(JSON.stringify(res));
    });
  }

  selectCase(selectedCase) {
    this.selectedId = selectedCase.CaseID;
    console.log(selectedCase);
    // return true;
  }
}
