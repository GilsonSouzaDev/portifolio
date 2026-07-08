import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ContactService, ContactMessageDto } from './contact';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ContactService', () => {
  let service: ContactService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ContactService,
        provideZonelessChangeDetection()
      ]
    });
    service = TestBed.inject(ContactService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should send a message via POST', () => {
    const mockMessage: ContactMessageDto = { 
      name: 'Test', 
      email: 'test@test.com', 
      subject: 'Assunto', 
      message: 'Hello' 
    };
    
    service.sendMessage(mockMessage).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne('/api/contact');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockMessage);
    req.flush({ message: 'Mensagem enviada com sucesso.' });
  });
});
