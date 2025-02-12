import {Component} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DomSanitizer} from '@angular/platform-browser';
import {ImageCroppedEvent, ImageCropperComponent} from 'ngx-image-cropper';

@Component({
  selector: 'app-crop-image',
  imports: [
    ImageCropperComponent
  ],
  templateUrl: './crop-image.component.html',
  styleUrl: './crop-image.component.css'
})
export class CropImageComponent {
  imageChangedEvent: any = '';
  croppedImage: any = '';
  showCropper = false;
  isUploading = false;
  errorMessage = '';
  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer
  ) {
  }

  fileChangeEvent(event: any): void {
    const file = event.target.files[0];
    if (file.size > 5 * 1024 * 1024) { // 5MB
      this.errorMessage = 'File quá lớn (tối đa 5MB)';
      return;
    }
    this.imageChangedEvent = event;
    this.showCropper = true;
  }

  imageCropped(event: ImageCroppedEvent) {
    this.croppedImage = this.sanitizer.bypassSecurityTrustUrl(event.objectUrl!);
  }

  private dataURLtoFile(dataurl: string): File {
    const arr = dataurl.split(',');
    const mime = arr[0].match(/:(.*?);/)![1];
    const bstr = atob(arr[1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);

    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], 'avatar.png', {type: mime});
  }

  uploadAvatar() {
    const file = this.dataURLtoFile(this.croppedImage.changingThisBreaksApplicationSecurity);
    const formData = new FormData();
    formData.append('file', file);

    this.http.put('/api/user/avatar', formData).subscribe({
      next: (res) => {
        this.isUploading = false;
        this.showCropper = false;
      },
      error: (err) => {
        this.isUploading = false;
        this.errorMessage = err.error?.message || 'Upload failed';
      }
    });
  }
}
