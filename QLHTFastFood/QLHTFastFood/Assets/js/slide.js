var slideIndex = 0;
showSlides();


function showSlides() {
  var i;
  var slides = document.getElementsByClassName("slideshow_container-sub");
  var dots = document.getElementsByClassName("dot");
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";  
  }
  slideIndex++;
  if (slideIndex > slides.length) {slideIndex = 1}    
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block";  
  dots[slideIndex-1].className += " active";
  setTimeout(showSlides, 4000); // Change image every 2 seconds
}

$('input[name="paymentMethod"]').off('click').on('click', function () {
    if ($(this).val() == 'NL') {
        $('#nganLuongContent').show();
    }
    else if ($(this).val() == 'ATM_ONLINE') {
        $('#bankContent').show();
    }
    else {
        $('#nganLuongContent').hide();
        $('#bankContent').hide();
    }
});

$(function () {
    $('#AlertBox').removeClass('hide');
    $('#AlertBox').delay(3000).slideUp(500);
});
setTimeout(function () {
    $('#AlertBox').delay(3000).slideUp(500);
}, 2000);
    



   