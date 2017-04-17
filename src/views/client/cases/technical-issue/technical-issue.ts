export class TechnicalIssue {
  cases: {}[];
  selectedId: number;

  constructor() {
    this.cases = [
      {
        id: 1,
        text: 'Wymiana żarówki'
      },
      {
        id: 2,
        text: 'Naprawa łóżka'
      },
      {
        id: 3,
        text: 'Naprawa telewizora'
      },
      {
        id: 4,
        text: 'Inne'
      }
    ]
  }

  selectCase(selectedCase) {
    this.selectedId = selectedCase.id;
    console.log(selectedCase);
    // return true;
  }
}
