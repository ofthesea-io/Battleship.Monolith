import { TestBed } from '@angular/core/testing';

import { BattleshipService } from './battleship.service';

describe('BattleshipService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BattleshipService = TestBed.get(BattleshipService);
    expect(service).toBeTruthy();
  });
});
