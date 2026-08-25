import { Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';

@Pipe({
  name: 'imageUrl',
  standalone: true
})
export class ImageUrlPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '';

    // Se a imagem ja comeca com http, precisamos verificar se e do ambiente local antigo
    if (value.startsWith('http://localhost:5217')) {
      // Trocar pela base URL correta (sem o /api)
      const baseUrl = environment.apiUrl.replace('/api', '');
      return value.replace('http://localhost:5217', baseUrl);
    }

    // Se for uma url relativa
    if (value.startsWith('/images/') || value.startsWith('/assets/')) {
      const baseUrl = environment.apiUrl.replace('/api', '');
      return `${baseUrl}${value}`;
    }

    // Se for uma URL externa ou ja correta, retorna como esta
    return value;
  }
}
