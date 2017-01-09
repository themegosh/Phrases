(function () {
    "use strict";
    angular.module("Phrases").factory("SoundService", ['PhrasesService', '$rootScope', function (ps, $rootScope) {
        var ss = {};

        ss.forceReload = false;
        ss.isInit = false;

        ss.currentPhrase = {};
        ss.currentPhraseProgress = {};
        ss.audio = {};

        ss.init = function () {
            ss.isInit = true;
            ss.audio = $('#audio-ele')[0];
            //ss.audio.addEventListener('timeupdate', ss.onAudioTimeUpdate);
        }

        //this entire thing is gross. rewrite it at some point
        ss.playSound = function (phrase, shouldReload, $event) {

            if (!ss.isInit) {
                ss.init();
            }

            ss.currentPhrase = phrase;
            ss.currentPhraseProgress = $($event.currentTarget).find('.phrase-progress').first();
            ss.currentPhraseProgress.width('0%');

            $('.fab').fadeIn();
            ss.currentPhrase.loading = true;
            var url = "/api/audio/GetAudio/" + ss.currentPhrase.guid + "?userGuid=" + ps.user.userGuid;
            
            if ($("#audio-src").attr("src") != url || shouldReload || ss.forceReload) {
                $("#audio-src").attr("src", url);
                ss.audio.load();//load new file
                ss.forceReload = false;
            }
            ss.audio.pause();
            ss.audio.currentTime = 0;
            ss.audio.oncanplaythrough = function () {
                ss.currentPhraseProgress.stop().animate({ width: '100%' }, ss.audio.duration * 1000, 'linear', function () { ss.currentPhraseProgress.width('0%') });
                ss.onCanPlayThrough();
            };
            
            ss.audio.addEventListener('ended', function () {
                ss.updateFab();
            });
        }

        ss.onCanPlayThrough = function () {
            ss.currentPhrase.loading = false;
            ss.audio.play().catch(function (e) {
                showNotification("Hey!", "Looks like your browser blocked the audio playback. Click [Play] again!", "info");
                console.log("Browser blocked the initial audio playback... TODO: FIX THIS.");
                console.log(e);
            });
            ss.updateFab();
            $rootScope.$apply();
        }

        ss.onAudioTimeUpdate = function () {
            if (ss.audio.currentTime != ss.audio.duration && ss.audio.currentTime != 0) {
                ss.currentPhrase.playProgress = ((ss.audio.currentTime / ss.audio.duration) * 100) + "%";
            } else {
                ss.currentPhrase.playProgress = null;
            }

            //console.log("TIME UPDATED: " + ss.audio.currentTime + " OF " + ss.audio.duration + ": " + ss.currentPhrase.playProgress);

            $rootScope.$apply();
        }

        ss.playPause = function () {
            if (ss.audio.duration > 0 && !ss.audio.paused) {
                ss.audio.pause();
            } else {
                ss.audio.currentTime = 0;
                ss.audio.oncanplaythrough = ss.onCanPlayThrough();
            }
            ss.updateFab();
        }

        ss.updateFab = function () {
            if (ss.audio.paused) {
                $('.fab').html('<i class="fa fa-play" aria-hidden="true"></i>');
            } else {
                $('.fab').html('<i class="fa fa-pause" aria-hidden="true"></i>');
            }
        }
        
        
        return ss;
    }]);
})();