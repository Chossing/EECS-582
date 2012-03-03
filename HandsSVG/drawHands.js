 function drawRightHand()
 {
    var handGroup= d3.select("#svgParent")
                                .append("svg:g")
                                    .attr("id", "rightHandTranslate")
                                        .append("svg:g")
                                            .attr("id",  "rightHandScale");
                    
        handGroup.append("svg:path")
            .attr("id", "rightHandOutline")
            .attr("d","M 35.526448,57.328433 C 46.482427,57.328433 56.839251,57.328433 56.839251,33.154759 C 56.861371,11.28334 52.032222,15.499819 49.735074,17.292139 L 49.588502,13.517498 C 49.479994,10.723096 41.447096,10.891693 41.439384,13.095197 L 41.419727,9.634018 C 41.398923,5.9712631 32.105945,6.3412943 32.028966,8.771618 L 31.879403,13.493491 C 31.79381,10.831509 24.572078,9.825891 24.356939,15.004346 C 24.356939,23.062237 24.441623,31.736007 24.570468,34.305887 C 20.804349,37.759268 16.653516,20.903873 9.3780604,23.997528 C 6.1255034,26.803402 18.793355,46.588402 20.162852,48.674821 C 21.470408,50.66686 27.309463,57.328433 35.526448,57.328433 z")
            .attr("stroke", "black")
            .attr("storke-width", "3");
        //.attr("style","fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:2.51114988;stroke-linecap:butt;stroke-linejoin:round;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1");

        /*
        handGroup.append("svg:path")
        .attr("d","M 40.855307,10.132212 L 40.855307,30.852504"  )
        .attr("style","fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:1.26577067;stroke-linecap:butt;stroke-linejoin:miter;marker:none;stroke-miterlimit:4;stroke-dasharray:none;stroke-dashoffset:0;stroke-opacity:1;visibility:visible;display:inline;overflow:visible;font-family:Bitstream Vera Sans" );
        handGroup.append("svg:path")
        .attr("d","M 32.678295,10.132212 L 32.504316,32.003632")
        .attr("style","fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:1.26577067;stroke-linecap:butt;stroke-linejoin:miter;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1" );
        handGroup.append("svg:path")
        .attr("d", "M 28.328821,38.334832 C 28.328821,38.334832 35.287979,39.48596 38.071643,50.997233")
        .attr("style","fill:none;fill-opacity:0.75;fill-rule:evenodd;stroke:#000000;stroke-width:1.26577067px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1"  );
        handGroup.append("svg:path")
        .attr("d", "M 49.206297,15.887849 L 49.206297,33.29865")
        .attr("style","fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:1.26577067;stroke-linecap:butt;stroke-linejoin:miter;marker:none;stroke-miterlimit:4;stroke-dasharray:none;stroke-dashoffset:0;stroke-opacity:1;visibility:visible;display:inline;overflow:visible;font-family:Bitstream Vera Sans" );
        handGroup.append("svg:path")
        .attr("d", "M 52.859855,35.600905 C 46.944571,30.300922 35.635938,30.540741 29.546674,32.291414")
        .attr("style", "fill:none;fill-opacity:0.75;fill-rule:evenodd;stroke:#000000;stroke-width:1.26577067px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1" );
        */
 } 
 function drawLeftHand()
 {
 var handGroup= d3.select("#svgParent")
                                .append("svg:g")
                                    .attr("id", "leftHandTranslate")
                                        .append("svg:g")
                                            .attr("id",  "leftHandScale");
                    
        handGroup.append("svg:path")
            .attr("id", "leftHandOutline")
            .attr("d","M 30.158911,57.328433 C 19.202932,57.328433 8.8461083,57.328433 8.8461083,33.154759 C 8.8239883,11.28334 13.653137,15.499819 15.950285,17.292139 L 16.096857,13.517498 C 16.205365,10.723096 24.238263,10.891693 24.245975,13.095197 L 24.265632,9.634018 C 24.286436,5.9712631 33.579414,6.3412943 33.656393,8.771618 L 33.805956,13.493491 C 33.891549,10.831509 41.113281,9.825891 41.32842,15.004346 C 41.32842,23.062237 41.243736,31.736007 41.114891,34.305887 C 44.88101,37.759268 49.031843,20.903873 56.307299,23.997528 C 59.559856,26.803402 46.892004,46.588402 45.522507,48.674821 C 44.214951,50.66686 38.375896,57.328433 30.158911,57.328433 z")
            .attr("stroke", "black")
            .attr("storke-width", "3");

//        .attr("style","fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:2.51114988;stroke-linecap:butt;stroke-linejoin:round;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1" );
        
 }
