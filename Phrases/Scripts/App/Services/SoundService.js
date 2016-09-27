(function () {
    "use strict";
    angular.module("Phrases").factory("SoundService", [function () {
        var ss = {};

        ss.playSound = function (guid, shouldReload) {
            $('.fab').fadeIn();

            var url = "api/tts/GetAudio?id=" + guid + ".mp3";
            var audio = $('#audio-ele');
            if ($("#audio-src").attr("src") != url || shouldReload) {
                $("#audio-src").attr("src", url);
                audio[0].load();//load new file
            }
            audio[0].pause();
            audio[0].currentTime = 0;
            audio[0].oncanplaythrough = audio[0].play();

            audio[0].addEventListener('ended', function () {
                ss.updateFab();
            });

            ss.updateFab();
        }

        ss.playPause = function () {
            var audio = $('#audio-ele');
            if (audio[0].duration > 0 && !audio[0].paused) {
                audio[0].pause();
            } else {
                audio[0].currentTime = 0;
                audio[0].oncanplaythrough = audio[0].play();
            }
            ss.updateFab();
        }

        ss.updateFab = function () {
            if ($('#audio-ele')[0].paused) {
                $('.fab').html('<i class="fa fa-play" aria-hidden="true"></i>');
            } else {
                $('.fab').html('<i class="fa fa-pause" aria-hidden="true"></i>');
            }
        }
        
        
        return ss;
    }]);
})();